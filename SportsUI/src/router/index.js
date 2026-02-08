import { createRouter, createWebHistory } from 'vue-router'
import HomePage from '../components/HomePage.vue'
import LeagueList from '../components/leagues/LeagueList.vue'
import LeagueDetail from '../components/leagues/LeagueDetail.vue'
import LeagueForm from '../components/leagues/LeagueForm.vue'
import TeamList from '../components/teams/TeamList.vue'
import TeamDetail from '../components/teams/TeamDetail.vue'
import TeamForm from '../components/teams/TeamForm.vue'
import PlayerList from '../components/players/PlayerList.vue'
import PlayerDetail from '../components/players/PlayerDetail.vue'
import PlayerForm from '../components/players/PlayerForm.vue'
import MatchList from '../components/matches/MatchList.vue'
import MatchDetail from '../components/matches/MatchDetail.vue'
import MatchForm from '../components/matches/MatchForm.vue'

const routes = [
  { path: '/', redirect: '/home' },
  { path: '/home', name: 'Home', component: HomePage },

  { path: '/leagues', name: 'Leagues', component: LeagueList },
  { path: '/leagues/create', name: 'LeagueCreate', component: LeagueForm },
  { path: '/leagues/:id', name: 'LeagueDetail', component: LeagueDetail },
  { path: '/leagues/:id/edit', name: 'LeagueEdit', component: LeagueForm },

  { path: '/teams', name: 'Teams', component: TeamList },
  { path: '/teams/create', name: 'TeamCreate', component: TeamForm },
  { path: '/teams/:id', name: 'TeamDetail', component: TeamDetail },
  { path: '/teams/:id/edit', name: 'TeamEdit', component: TeamForm },

  { path: '/players', name: 'Players', component: PlayerList },
  { path: '/players/create', name: 'PlayerCreate', component: PlayerForm },
  { path: '/players/:id', name: 'PlayerDetail', component: PlayerDetail },
  { path: '/players/:id/edit', name: 'PlayerEdit', component: PlayerForm },

  { path: '/matches', name: 'Matches', component: MatchList },
  { path: '/matches/create', name: 'MatchCreate', component: MatchForm },
  { path: '/matches/:id', name: 'MatchDetail', component: MatchDetail },
  { path: '/matches/:id/edit', name: 'MatchEdit', component: MatchForm },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
